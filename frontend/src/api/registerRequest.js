import API_URL from "../appConfig";

export default async function registerRequest(formData){
    return fetch(`${API_URL}/clients/register-client`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Name: formData.firstName,
          Surname: formData.lastName,
          Email: formData.email,
          Password: formData.password,
          BirthDate: formData.birthDate,
          PhoneNumber: formData.phoneNumber,
          Address: formData.address,
        }),
      });
}