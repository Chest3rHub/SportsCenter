import API_URL from "../appConfig";

const registerEmployeeRequest = async function(formData) {
    const currentDate = new Date().toISOString();
    
    return fetch(`${API_URL}/Employees/Register`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          name: formData.firstName,
          surname: formData.lastName,
          email: formData.email,
          password: formData.password,
          birthDate: formData.birthDate,
          phoneNumber: formData.phoneNumber,
          address: formData.address,
          positionName: formData.position,
          hireDate: currentDate
        }),
        credentials: 'include'
    });
};

export default registerEmployeeRequest;