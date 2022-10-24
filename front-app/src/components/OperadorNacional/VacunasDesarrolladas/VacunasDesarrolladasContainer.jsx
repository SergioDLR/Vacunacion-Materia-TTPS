import { Container, Button, Box, CircularProgress } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useState, useEffect, useContext } from "react";
import VacunaDesarrolladaTabla from "./VacunaDesarrolladaTabla";
import RegistrarVacunaDesarrollada from "./RegistrarVacunaDesarrollada/RegistrarVacunaDesarrollada";
import allUrls from "@/services/backend_url";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
import axios from "axios";
const VacunasDesarrolladasContainer = () => {
  const [open, setOpen] = useState(false);
  const [vacunasDesarrolladas, setVacunasDesarrolladas] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const { userSesion } = useContext(UserContext);
  useEffect(() => {
    cargarVacunasDesarrolladas();
  }, []);

  const cargarVacunasDesarrolladas = () => {
    try {
      axios
        .get(`${allUrls.todasVacunasDesarrolladas}?emailOperadorNacional=${userSesion.email}`)
        .then((response) => {
          setVacunasDesarrolladas(response?.data?.listaVacunasDesarrolladasDTO);
          setEstaCargando(false);
        })
        .catch((error) => {
          alert.error(`Ocurrio un error ${error}`);
        });
    } catch (error) {
      alert.error(`Ocurrio un error del lado del servidor: ${error}`);
    }
  };
  return (
    <Container>
      {!estaCargando ? (
        <>
          <Button variant={"contained"}>Comprar</Button>
          <Button variant={"contained"}>Distribuir</Button>
          <CustomModal title="Cargar vacuna desarrollada" open={open} setOpen={setOpen}>
            <RegistrarVacunaDesarrollada setOpen={setOpen} cargarVacunasDesarrolladas={cargarVacunasDesarrolladas} />
          </CustomModal>
          <VacunaDesarrolladaTabla vacunasDesarrolladas={vacunasDesarrolladas} />
        </>
      ) : (
        <Box sx={{ marginTop: 1 }}>
          <CircularProgress sx={{ display: "table", margin: "auto" }} />
        </Box>
      )}
    </Container>
  );
};

export default VacunasDesarrolladasContainer;
