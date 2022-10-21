import { Paper, Button, Container } from "@mui/material";
import { useState } from "react";
import RegistrarVacuna from "./RegistraVacuna";
import SelectPandemia from "./RegistrarVacunaForm/SelectPandemia";
import RegistrarVacunaContainer from "./RegistrarVacunaForm/RegistrarVacunaContainer";
import CustomModal from "../../utils/Modal";
const VacunasListContainer = () => {
  const [open, setOpen] = useState(false);
  return (
    <Container>
      <Paper sx={{ padding: 3 }}>
        <Button variant={"contained"}>Comprar</Button>
        <Button variant={"contained"}>Distribuir</Button>
        <CustomModal title={"Registrar vacuna"}>
          <RegistrarVacunaContainer />
        </CustomModal>
      </Paper>
    </Container>
  );
};

export default VacunasListContainer;
