import API_URL from "../appConfig";

const selfAddTask = (taskData) => {
  return fetch(`${API_URL}/Employees/Self-Add-task`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(taskData),
    credentials: 'include'
  });
};

export default selfAddTask;