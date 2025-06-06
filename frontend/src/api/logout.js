import API_URL from "../appConfig";

const logout = () => {
  return fetch(`${API_URL}/Users/logout`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    credentials: 'include'
  });
};

export default logout;