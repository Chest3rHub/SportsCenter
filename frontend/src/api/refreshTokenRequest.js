import API_URL from "../appConfig";

export default async function refreshTokenRequest(token){
    

    return fetch(`${API_URL}/Users/login`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
      });
}