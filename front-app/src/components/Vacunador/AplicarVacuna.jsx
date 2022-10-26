import { Box } from "@mui/system";
import { TextField, Paper, Container } from "@mui/material";
import CustomButton from "../utils/CustomButtom";
import { useState, useContext } from "react";
import { UserContext } from "../Context/UserContext";
import axios from "axios";
import allUrls from "@/services/backend_url";
import CustomLoader from "../utils/CustomLoader";
import { useAlert } from "react-alert";
import FormularioVacunacion from "./FormularioVacunacion";
import CustomModal from "../utils/Modal";
const AplicarVacuna = () => {
  const [dni, setDni] = useState(-1);
  const { userSesion } = useContext(UserContext);
  const alert = useAlert();
  const [estaCargando, setEstaCargando] = useState(false);
  const [persona, setPersona] = useState({});
  const [open, setOpen] = useState(false);
  const handleChangeDni = (evt) => {
    setDni(evt.target.value);
  };
  const handleSubmit = (evt) => {
    evt.preventDefault();
    setEstaCargando(true);
    try {
      axios
        .get(`${allUrls.apiRenaper}${dni}`)
        .then((response) => {
          setPersona(response.data);
          setOpen(true);
        })
        .catch(() => alert.error("No se encontro la persona"))
        .finally(() => setEstaCargando(false));
    } catch (e) {
      alert.error("Error interno del servidor renaper");
    }
  };
  return (
    <Container>
      {estaCargando && <CustomLoader></CustomLoader>}
      <Box width={300} sx={{ marginTop: 3, marginLeft: "auto", marginRight: "auto" }}>
        <Paper sx={{ padding: 2 }}>
          <form onSubmit={handleSubmit}>
            <TextField
              sx={{ display: "block" }}
              fullWidth
              id="dni-field"
              label="Dni:"
              variant="filled"
              type="number"
              required
              onChange={handleChangeDni}
            />
            <CustomButton color={"success"} type="submit" sx={{ marginTop: 1 }} variant="contained">
              Vacunar
            </CustomButton>
          </form>
          <CustomModal open={open} setOpen={setOpen} displayButton={false}>
            <FormularioVacunacion persona={persona} email={userSesion.email} setOpenPadre={setOpen} />
          </CustomModal>
        </Paper>
      </Box>
    </Container>
  );
};

export default AplicarVacuna;
