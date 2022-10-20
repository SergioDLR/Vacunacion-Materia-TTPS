import { Paper, Button, Container } from "@mui/material";

const VacunasListContainer = () => {
  return (
    <Container>
      <Paper sx={{ padding: 3 }}>
        <Button variant={"contained"}>Comprar</Button>
        <Button variant={"contained"}>Distribuir</Button>
      </Paper>
    </Container>
  );
};

export default VacunasListContainer;
