import API_URL from "../appConfig";

const getYourTasks = (token) => {
  return fetch(`${API_URL}/Employees/get-your-tasks`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },
    credentials: 'include',
  });
};

export default getYourTasks;