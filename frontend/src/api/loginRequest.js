import API_URL from "../appConfig";

export default async function loginRequest(loginData){
    return fetch(`${API_URL}/Users/login`, {

        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: loginData.email,
          password: loginData.password,
        }) 
      });
}