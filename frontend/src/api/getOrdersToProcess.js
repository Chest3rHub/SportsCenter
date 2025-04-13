import API_URL from "../appConfig";

export default function getOrdersToProcess(token) {
  return fetch(`${API_URL}/Orders/Get-orders-to-process`, {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
  });
}