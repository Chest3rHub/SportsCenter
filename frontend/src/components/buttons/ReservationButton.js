import React from 'react';
import { Button} from '@mui/material';
import { darken } from '@mui/system';


const ReservationButton = ({ backgroundColor, onClick, children, minWidth, disabled }) => {
    const darkenedColor = darken(backgroundColor, 0.2);
    return (
        <Button
            onClick={onClick}
            disabled={disabled}
            sx={{backgroundColor: backgroundColor,                                  
                borderRadius: '5px', 
                border:"none", 
                boxShadow: '0 5px 5px rgba(0, 0, 0, 0.6)',   
                borderRadius: '20px',
                fontSize: '0.8rem',
                cursor: 'pointer',
                fontWeight: 'bold',
                minWidth: minWidth ? minWidth :"6vw",
                paddingTop:'7px',
                paddingTop:'0.6rem',
                paddingBottom:'0.4rem',   
                color:'black',
                marginLeft:'1vw',
                '&:hover': {
                    backgroundColor: darkenedColor, 
                }
                }}
        >
            {children}
        </Button>
    );
};

export default ReservationButton;
