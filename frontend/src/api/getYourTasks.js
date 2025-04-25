import API_URL from "../appConfig";

const getYourTasks = (offset) => {
  return fetch(`${API_URL}/Employees/get-your-tasks?offset=${offset}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    },
    credentials: 'include'
  });
};

export default getYourTasks;