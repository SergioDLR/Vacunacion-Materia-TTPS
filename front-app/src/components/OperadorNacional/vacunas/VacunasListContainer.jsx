import { Paper, Button, Container, Typography, Box, CircularProgress } from "@mui/material";
import { useState, useEffect, useContext } from "react";
import RegistrarVacuna from "./RegistraVacuna";
import SelectPandemia from "./RegistrarVacunaForm/SelectPandemia";
import RegistrarVacunaContainer from "./RegistrarVacunaForm/RegistrarVacunaContainer";
import CustomModal from "../../utils/Modal";
import { UserContext } from "../../Context/UserContext";
import allUrls from "../../../services/backend_url";
import { useAlert } from "react-alert";
import TablaVacunas from "./TablaVacunas";
import { cargarVacunas } from "@/services/getVacunas";
const VacunasListContainer = () => {
  const [vacunasCreadas, setVacunasCreadas] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const { userSesion } = useContext(UserContext);
  const [open, setOpen] = useState(false);
  const alert = useAlert();
  useEffect(() => {
    cargarTodasLasVacunas();
  }, []);

  const cargarTodasLasVacunas = () => {
    cargarVacunas(setVacunasCreadas, allUrls.todasVacunas, userSesion.email, alert, () => setEstaCargando(false));
  };
  return (
    <Container>
      <CustomModal title={"Registrar vacuna"} open={open} setOpen={setOpen}>
        <RegistrarVacunaContainer cargarTodasLasVacunas={cargarTodasLasVacunas} setOpen={setOpen} />
      </CustomModal>
      {estaCargando ? (
        <Box sx={{ marginTop: 1 }}>
          <CircularProgress sx={{ display: "table", margin: "auto" }} />
        </Box>
      ) : (
        <TablaVacunas vacunas={vacunasCreadas} />
      )}
    </Container>
  );
};

export default VacunasListContainer;
