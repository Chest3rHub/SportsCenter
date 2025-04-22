import API_URL from "../appConfig";

export default async function getAccountInfo(token){
    return fetch(`${API_URL}/users/account-info`, { 
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}