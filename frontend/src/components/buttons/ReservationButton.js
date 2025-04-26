import React from 'react';
import { Button } from '@mui/material';

export default function ReservationButton({ type, children, onClick, style, hoverBackgroundColor }) {
    return (
        <Button 
            type={type} 
            onClick={onClick} 
            sx={{
                backgroundColor: '#FFE3B3',
                color: 'black',
                display: 'inline-block',
                boxShadow: '0 5px 5px rgba(0, 0, 0, 0.6)',
                border: 'none',
                borderRadius: '20px',
                padding: '0.5rem 1.2rem',
                fontSize: '1rem',
                fontWeight: 'bold', 
                cursor: 'pointer',
                '&:hover': {
                    backgroundColor: hoverBackgroundColor ? hoverBackgroundColor : '#E0C28B',
                },
                ...style,
            }}
        >
            {children}
        </Button>
    );
}
