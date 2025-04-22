import API_URL from "../appConfig";

export default async function refreshTokenRequest(token){
    

    return fetch(`${API_URL}/Users/refresh-token`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}