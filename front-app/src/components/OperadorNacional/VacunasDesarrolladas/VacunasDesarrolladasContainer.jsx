import { Container, Button } from "@mui/material";
import CustomModal from "@/components/utils/Modal";

const VacunasDesarrolladasContainer = () => {
  return (
    <Container>
      <Button variant={"contained"}>Comprar</Button>
      <Button variant={"contained"}>Distribuir</Button>
      <CustomModal tilte="Cargar vacuna desarrollada"></CustomModal>
    </Container>
  );
};

export default VacunasDesarrolladasContainer;
