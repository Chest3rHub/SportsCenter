import React from "react";
import { TextField } from "@mui/material";

export default function CustomTimeInput(props) {

  // Funkcja obsługująca zmianę czasu
  const handleTimeChange = (event) => {
    const value = event.target.value;
    const [hours, minutes] = value.split(":");

    // Akceptujemy tylko pełne godziny (00) lub półgodziny (30)
    if (minutes !== "00" && minutes !== "30") {
      event.target.value = `${hours}:00`; // Ustawiamy na pełną godzinę, jeżeli minuty są inne
    }

    // Wywołanie funkcji onChange
    props.onChange(event);
  };

  // Formatowanie czasu - wyświetlamy tylko pełne godziny lub półgodziny
  const formatTime = (time) => {
    const [hours, minutes] = time.split(":");
    return `${hours}:${minutes === "00" || minutes === "30" ? minutes : "00"}`; // Wymuszamy tylko pełne godziny i półgodziny
  };

  return (
    <TextField
      {...props}
      type="time"
      fullWidth
      value={formatTime(props.value)}  // Wyświetlamy czas z pełnymi godzinami lub półgodzinami
      onChange={handleTimeChange}  // Obsługuje zmianę wartości
      error={props.error}
      helperText={props.error ? props.helperText : ""}
      inputProps={{
        step: 1800, // Tylko pełne godziny i półgodziny
      }}
      required
      size="small"
      InputLabelProps={{ shrink: true }}
      sx={{
        "& .MuiOutlinedInput-root": {
          borderRadius: "3rem",
          backgroundColor: "white",
          "& input": {
            borderRadius: "3rem",
            // Ukryj inne minuty, aby nie były widoczne w UI
            "-webkit-appearance": "none",
            "-moz-appearance": "textfield",
          },
          "& fieldset": {
            borderColor: props.error ? "red" : "transparent",
          },
          "&:hover fieldset": {
            borderColor: props.error ? "red" : "transparent",
          },
          "&.Mui-focused fieldset": {
            borderColor: props.error ? "red" : "#1976d2",
          },
          ...(props.readonlyStyle && {
            "&.Mui-focused fieldset": {
              borderColor: "transparent",
              outline: "none",
            },
          }),
        },
        ...props.additionalStyles,
      }}
    />
  );
}
