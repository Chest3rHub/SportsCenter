import API_URL from "../appConfig";

const getYourTasks = (token) => {
  return fetch(`${API_URL}/Employees/get-your-tasks`, {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
  });
};

export default getYourTasks;