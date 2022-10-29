import CustomButton from "@/components/utils/CustomButtom";
import { Container } from "@mui/system";
import { useEffect, useState, useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import axios from "axios";
import allUrls from "@/services/backend_url";
import TableDistribuciones from "./TableDistribuciones";
import CustomLoader from "@/components/utils/CustomLoader";
import CustomModal from "@/components/utils/Modal";
import { useAlert } from "react-alert";
import CrearDistribucion from "./CrearDistribucion";
import { Box } from "@mui/material";
const DistribucionesContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [distribuciones, setDistribuciones] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [open, setOpen] = useState(false);
  const [jurisdicciones, setJurisdicciones] = useState([]);
  const alert = useAlert();
  useEffect(() => {
    try {
      cargarDistribuciones();
      axios.get(`${allUrls.jurisdiccion}GetAll`).then((response) => setJurisdicciones(response.data));
    } catch (e) {
      alert.error(`Ocurrio un error del lado del servidor ${e}`);
    }
  }, []);

  const cargarDistribuciones = () => {
    axios
      .get(`${allUrls.visualizarDistribuciones}?emailOperadorNacional=${userSesion.email}`)
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          setDistribuciones(response?.data?.distribuciones);
        } else {
          alert.error(`Ocurrio un error`);
        }
      })
      .catch(() => alert.error("Ocurrio un error"))
      .finally(() => setEstaCargando(false));
  };
  return (
    <Container>
      {estaCargando && <CustomLoader />}
      <Box sx={{ marginTop: 1, marginBottom: 1 }}>
        <CustomModal open={open} setOpen={setOpen} title="Distribuir">
          Distribuir a una jurisdiccion
          <CrearDistribucion
            jurisdicciones={jurisdicciones}
            setOpenMain={setOpen}
            cargarDistribuciones={cargarDistribuciones}
          />
        </CustomModal>
      </Box>
      <TableDistribuciones distribuciones={distribuciones} />
    </Container>
  );
};

export default DistribucionesContainer;
