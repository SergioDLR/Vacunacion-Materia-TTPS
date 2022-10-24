import { Container, Button } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useState } from "react";
import RegistrarVacunaDesarrollada from "./RegistrarVacunaDesarrollada/RegistrarVacunaDesarrollada";
const VacunasDesarrolladasContainer = () => {
  const [open, setOpen] = useState(false);
  return (
    <Container>
      <Button variant={"contained"}>Comprar</Button>
      <Button variant={"contained"}>Distribuir</Button>
      <CustomModal tilte="Cargar vacuna desarrollada" open={open} setOpen={setOpen}>
        <RegistrarVacunaDesarrollada />
      </CustomModal>
    </Container>
  );
};

export default VacunasDesarrolladasContainer;
