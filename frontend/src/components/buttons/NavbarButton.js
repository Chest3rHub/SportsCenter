import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function NavbarButton(props) {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(props.navigate);
    };

    return (
        <Button
            onClick={handleClick}
            sx={{
                background: 'linear-gradient(to right, #0AB68B, #FFE3B3)',
                border: '1px solid #2D7E5C',
                borderRadius: '30px',
                padding: '0 20px',
                fontSize: '1rem',
                color:'black',
                cursor: 'pointer',
                paddingTop:'8px',
                paddingBottom:'4px',
                minWidth:'11vw',
                '&:hover': {
                    background: 'linear-gradient(to right, #FFE3B3, #0AB68B)',
                },
            }}
        >
            {props.children}
        </Button>
    );
}
