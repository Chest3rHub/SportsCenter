import React from 'react';
import { Button} from '@mui/material';
import { darken } from '@mui/system';


const NewsButton = ({ backgroundColor, onClick, children, minWidth }) => {
    const darkenedColor = darken(backgroundColor, 0.2);
    return (
        <Button
            onClick={onClick}
            sx={{backgroundColor: backgroundColor,                        
                padding: "3px 8px",               
                borderRadius: '5px', 
                border:"none", 
                boxShadow: '0 5px 5px rgba(0, 0, 0, 0.6)',   
                borderRadius: '20px',
                fontSize: '0.8rem',
                cursor: 'pointer',
                fontWeight: 'bold',
                minWidth: minWidth ? minWidth :"6vw",
                paddingTop:'7px',
                color:'black',
                '&:hover': {
                    backgroundColor: darkenedColor, 
                }
                }}
        >
            {children}
        </Button>
    );
};

export default NewsButton;
