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
import { Box, MenuItem, Select } from "@mui/material";
const DistribucionesContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [distribuciones, setDistribuciones] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [open, setOpen] = useState(false);
  const [jurisdicciones, setJurisdicciones] = useState([]);
  const [lotes, setLotes] = useState([]);
  const [selectedLote, setSelectedLote] = useState(0);
  const alert = useAlert();
  useEffect(() => {
    try {
      cargarDistribuciones();
      cargarLotes();

      axios.get(`${allUrls.jurisdiccion}GetAll`).then((response) => setJurisdicciones(response.data));
    } catch (e) {
      alert.error(`Ocurrio un error del lado del servidor ${e}`);
    }
  }, []);
  console.log(lotes);
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

  const cargarLotes = () => {
    axios.get(allUrls.displayLotes).then((response) => setLotes(response.data));
  };

  const handleDeleteLote = () => {
    axios
      .post(`${allUrls.vencerLote}?email=${userSesion.email}&idLote=${selectedLote}`)
      .then((response) => alert.success("El lote se vencio con exito"));
    cargarLotes();
  };

  const handleChangeLote = (evt) => {
    setSelectedLote(evt.target.value);
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
      <h4>Vencer un lote:</h4>
      <Select
        labelId="Selecciona_lote"
        id="tipo-de-pandemia-select"
        value={selectedLote}
        label="Selecciona un lote"
        onChange={handleChangeLote}
      >
        <MenuItem value={0}>Selecciona un lote </MenuItem>
        {lotes.map((element, index) => (
          <MenuItem key={index} value={element}>
            {element}
          </MenuItem>
        ))}
      </Select>
      {selectedLote !== 0 && (
        <CustomButton onClick={handleDeleteLote} variant={"outlined"} color={"info"} textColor={"#2e7994"}>
          Vencer lote
        </CustomButton>
      )}
    </Container>
  );
};

export default DistribucionesContainer;
