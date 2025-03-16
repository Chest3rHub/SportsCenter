import API_URL from "../appConfig";

export default async function getNews(){
    return fetch(`${API_URL}/News/Get-news`, {

        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
}