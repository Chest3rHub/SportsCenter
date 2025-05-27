import API_URL from "../appConfig";

export default function markClientReservationAsPaid(email, reservationId) {
  return fetch(`${API_URL}/Reservation/pay-for-client-reservation`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ReservationId:reservationId, ClientEmail: email}),
    credentials: 'include'
  });
};