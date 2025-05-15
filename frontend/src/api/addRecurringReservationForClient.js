import API_URL from "../appConfig";

export default async function addRecurringReservationForClient(formData, token){
    return fetch(`${API_URL}/Reservation/Create-recurring-reservation`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          clientId: formData.clientId,
          CourtId: formData.courtId,
          StartTime: formData.startTime,
          EndTime: formData.endTime,
          CreationDate: formData.creationDate,
          TrainerId: formData.trainerId,
          ParticipantsCount: formData.participantsCount,
          IsEquipmentReserved: formData.isEquipmentReserved,
          Recurrence: formData.recurrence,
          RecurrenceEndDate: formData.recurrenceEndDate,
        }),
        credentials: 'include',
      });
}