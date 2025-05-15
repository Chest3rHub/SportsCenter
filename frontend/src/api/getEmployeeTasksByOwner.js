
import API_URL from "../appConfig";

const getEmployeeTasksByOwner = (employeeId) => {
  return fetch(`${API_URL}/Employees/get-employee-tasks-by-owner?employeeId=${employeeId}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },
    credentials: 'include'
  });
};

export default getEmployeeTasksByOwner;