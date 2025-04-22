import API_URL from "../appConfig";

export default async function addClientDiscount(formData, token){
    return fetch(`${API_URL}/Clients/update-client-deposit`, { 
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          deposit: formData.deposit,
          email: formData.email,   
        }),
      });
}