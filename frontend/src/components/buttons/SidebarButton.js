import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function SidebarButton(props) {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(props.navigate);
    };

    return (
        <Button 
            onClick={handleClick}
            sx={{
                color: 'inherit', 
                textDecoration: 'none', 
                background: 'none', 
                padding: 0, 
                margin: 0, 
                border: 'none', 
                font: 'inherit', 
                lineHeight: 'inherit', 
            }}
        >
            {props.children}
        </Button>
    );
}
