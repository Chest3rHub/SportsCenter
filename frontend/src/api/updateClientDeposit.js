import API_URL from "../appConfig";

export default async function updateClientDeposit(formData, token){
    return fetch(`${API_URL}/Clients/update-client-deposit`, { 
        method: 'PUT',
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