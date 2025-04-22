import API_URL from "../appConfig";

export default async function addClientDiscount(formData, token){
    return fetch(`${API_URL}/Clients/add-deposit-to-client`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          deposit: formData.deposit,
          email: formData.email,   
        }),
        credentials: 'include',
      });
}