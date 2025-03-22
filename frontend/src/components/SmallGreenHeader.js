import { Box, Typography } from "@mui/material";
export default function SmallGreenHeader({children, width}){
    return (<>

    <Typography sx={{
        width: width,
        borderRadius: '20px',
        backgroundColor: '#AFEBBC',
        boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
        display: 'inline-block',
        padding:'0.3rem 0px',
        textAlign: 'center',
    }}>
        {children}
    </Typography>
    </>);

}