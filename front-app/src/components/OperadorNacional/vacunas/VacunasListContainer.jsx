import { Paper, Button, Container, Typography, Box, CircularProgress } from "@mui/material";
import { useState, useEffect, useContext } from "react";
import RegistrarVacuna from "./RegistraVacuna";
import SelectPandemia from "./RegistrarVacunaForm/SelectPandemia";
import RegistrarVacunaContainer from "./RegistrarVacunaForm/RegistrarVacunaContainer";
import CustomModal from "../../utils/Modal";
import axios from "axios";
import { UserContext } from "../../Context/UserContext";
import allUrls from "../../../services/backend_url";
import { useAlert } from "react-alert";
import TablaVacunas from "./TablaVacunas";
const VacunasListContainer = () => {
  const [vacunasCreadas, setVacunasCreadas] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const { userSesion } = useContext(UserContext);
  const alert = useAlert();
  useEffect(() => {
    cargarTodasLasVacunas();
  }, []);

  const cargarTodasLasVacunas = () => {
    try {
      axios
        .get(`${allUrls.todasVacunas}?emailOperadorNacional=${userSesion.email}`)
        .then((response) => {
          if (response?.data?.estadoTransaccion === "Aceptada") {
            setVacunasCreadas(response?.data?.listaVacunasDTO);
          } else {
            response?.data?.errores?.forEach((element) => alert.error(element));
          }
          setEstaCargando(false);
        })
        .catch((e) => {
          alert.error(e);
          setEstaCargando(false);
        });
    } catch (e) {
      alert.error("Ocurrio un error del lado del servidor");
    }
  };
  return (
    <Container>
      <Button variant={"contained"}>Comprar</Button>
      <Button variant={"contained"}>Distribuir</Button>
      <CustomModal title={"Registrar vacuna"}>
        <RegistrarVacunaContainer cargarTodasLasVacunas={cargarTodasLasVacunas} />
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
