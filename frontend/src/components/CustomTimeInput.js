import React from "react";
import { TextField } from "@mui/material";

export default function CustomTimeInput(props) {
  return (
    <TextField
      label={props.label}
      type={props.type}
      id={props.id}
      name={props.name}
      value={props.value}
      onChange={props.onChange}
      error={props.error}
      helperText={props.error ? props.helperText : ""}
      {...props}
      sx={{
        "& .MuiOutlinedInput-root": {
          borderRadius: "3rem",
          backgroundColor: "white",
          "& input": {
            borderRadius: "3rem",
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
