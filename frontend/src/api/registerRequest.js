import API_URL from "../appConfig";

export default async function registerRequest(formData){
    return fetch(`${API_URL}/clients`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          imie: formData.firstName,
          nazwisko: formData.lastName,
          email: formData.email,
          haslo: formData.password,
          dataUr: formData.birthDate,
          nrTel: formData.phoneNumber,
          adres: formData.address,
        }),
      });
}