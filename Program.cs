using System.Text;
using System.Net;
using HtmlAgilityPack;

FileStream OpenFileToRecording(string name)
{
    FileStream file = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write);
    
    file.Flush();

    return file;
}

void WriteStringToFile(string str, ref FileStream outFile)
{
    byte [] bytes = Encoding.ASCII.GetBytes(str + '\n');
    outFile.Write(bytes);
}

string GetHtml(string adress)
{
    try
    {
        if (adress.Substring(adress.Length - 4) == "html")
        {
            HtmlWeb website = new HtmlWeb();
            
            HtmlDocument doc = website.Load(adress);

            return doc.Text;
        }
        else
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true);

            return reader.ReadToEnd();
        }
    }
    catch
    {
        return "";
    }
}

string GetAdress(string htmlCode, ref int i)
{
    char quot = '"';
    string adress = "";

    i = htmlCode.IndexOf("<a", i);
   
    if (i != -1)
    {
        i = htmlCode.IndexOf("href", i);

        if (i != -1)
        {
            i += +6;

            adress = htmlCode.Substring(i, htmlCode.IndexOf(quot, i) - i);

            i += adress.Length;
        }
    }

    return adress;
}

bool DomainIsUsed(string adress)
{
    bool result = false;
    
    if (adress.IndexOf("http:") != -1 || adress.IndexOf("https:") != -1)
    {
        result = true;
    }

    return result;
}

bool CheckAdress(string adress, ref HttpStatusCode status)
{
    try
    {
        HttpWebRequest request = WebRequest.Create(adress) as HttpWebRequest;
        request.Method = "HEAD";

        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        status = response.StatusCode;
        return status < HttpStatusCode.BadRequest;
    }
    catch (WebException webException)
    {
        status = ((HttpWebResponse)webException.Response).StatusCode;
        if (status < HttpStatusCode.BadRequest)
        {
            return true;
        }
        return false;
    }
}

void CheckAndWriteResult(string adress, ref int correctLinksAmount, ref int incorrectLinksAmount, ref FileStream successOutFile, ref FileStream errorOutFile)
{
    HttpStatusCode status = HttpStatusCode.OK;
    if (CheckAdress(adress, ref status))
    {
        WriteStringToFile(adress + " " + (int)status, ref successOutFile);
        correctLinksAmount++;
    }
    else
    {
        WriteStringToFile(adress + " " + (int)status, ref errorOutFile);
        incorrectLinksAmount++;
    }
}

bool AdressIsVerified(string adress, ref List<string> verifiedAdresses)
{ 
    if (adress.IndexOf('/') == 0 || adress.IndexOf('?') == 0)
    {
        adress = adress.Substring(1);
    }

    if (verifiedAdresses.Find(x => x == adress) != null)
    {
        return true;
    }

    verifiedAdresses.Add(adress);

    return false;
}

bool IsAdressCorrect(string adress)
{
    if (adress.Length == 0 || adress == "#" || adress == "/" || (adress.Contains(':') && !adress.Contains("http")))
    {
        return false;
    }

    return true;
}

void CheckPage(string domain, string adressIn, ref List<string> verifiedAdresses, ref int correctLinksAmount, ref int incorrectLinksAmount,
               ref FileStream successOutFile, ref FileStream errorOutFile)
{ 
    string htmlCode = GetHtml(adressIn);

    string adress = adressIn;

    int i = 0;
    while (i != -1)
    { 
        if (adress.Contains("&amp"))
        {
            int pos = adress.IndexOf("&amp;");
            adress = adress.Substring(0, pos) + "&" + adress.Substring(pos + 5);
        }
        if (IsAdressCorrect(adress) && !DomainIsUsed(adress) && !AdressIsVerified(adress, ref verifiedAdresses))
        {
            if (adress.IndexOf("/") == 0 || adress.IndexOf("?") == 0)
            {
                CheckAndWriteResult(domain + adress, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
            }
            else if (adress.IndexOf("../") == 0)
            {
                CheckAndWriteResult(domain + '/' + adress.Substring(3), ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
            }
            else
            {
                CheckAndWriteResult(adressIn + '/' + adress, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
            }
        }

        adress = GetAdress(htmlCode, ref i);
    }
}

void CheckNestedAddresses(string domain, List<string> verifiedAdresses, ref int correctLinksAmount, ref int incorrectLinksAmount,
                          ref FileStream successOutFile, ref FileStream errorOutFile)
{
    for (int i = 0; i < verifiedAdresses.Count; i++)
    {
        if (verifiedAdresses[i].IndexOf('/') != 0)
        {
            CheckPage(domain, domain + '/' + verifiedAdresses[i], ref verifiedAdresses, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
        }
        else
        {
            CheckPage(domain, domain + verifiedAdresses[i], ref verifiedAdresses, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
        }
    };
}

void CheckAdresses(string domain, List<string> verifiedAdresses, ref int correctLinksAmount, ref int incorrectLinksAmount,
                   ref FileStream successOutFile, ref FileStream errorOutFile)
{
    CheckAndWriteResult(domain, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);

    CheckPage(domain, domain, ref verifiedAdresses, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);

    CheckNestedAddresses(domain, verifiedAdresses, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);
}


//string domain = "http://links.qatl.ru";
string domain = "https://statler.ru";
if (args.Length > 0)
{
    domain = args[0];
}

FileStream successOutFile = OpenFileToRecording("successURL.txt");
FileStream errorOutFile = OpenFileToRecording("errorURL.txt");

int correctLinksAmount = 0;
int incorrectLinksAmount = 0;

List<string> verifiedAdresses = new List<string>();

CheckAdresses(domain, verifiedAdresses, ref correctLinksAmount, ref incorrectLinksAmount, ref successOutFile, ref errorOutFile);

string amount = "" + correctLinksAmount;
WriteStringToFile(amount, ref successOutFile);

amount = "" + incorrectLinksAmount;
WriteStringToFile(amount, ref errorOutFile);

string dateNow = "" + DateTime.Now;
WriteStringToFile(dateNow, ref successOutFile);
WriteStringToFile(dateNow, ref errorOutFile);

successOutFile.Close();
errorOutFile.Close();