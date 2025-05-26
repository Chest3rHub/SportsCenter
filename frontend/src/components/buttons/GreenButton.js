import { Button } from '@mui/material';

export default function GreenButton({ type, children, onClick, style, hoverBackgroundColor, disabled }) {
    return (
        <Button 
            type={type} 
            onClick={onClick} 
            sx={{
                backgroundColor: '#8edfb4',
                color: 'black',
                display: 'block',
                boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
                border: 'none',
                borderRadius: '20px',
                padding: '0px',  
                fontSize: '1rem',
                cursor: 'pointer',
                width: '100%',
                fontWeight: 'bold',
                paddingTop:'0.5rem',
                paddingBottom:'0.3rem',
                '&:hover': {
                    backgroundColor: hoverBackgroundColor ? hoverBackgroundColor : '#7ec7a0',
                },
                ...style 
            }}
            disabled={disabled}
        >
            {children}
        </Button>
    );
}
