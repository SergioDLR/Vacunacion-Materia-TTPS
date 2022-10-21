import { useState, useEffect } from "react";
import allUrls from "../../../services/backend_url";
import axios from "axios";
import { useAlert } from "react-alert";
import { Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
const RegistrarVacuna = ({ tipoVacunaSelected, setTipoVacunaSelected }) => {
  const [tiposVacuna, setTiposVacuna] = useState([]);

  const alert = useAlert();

  useEffect(() => {
    try {
      axios.get(`${allUrls.vacunas}GetAll`).then((response) => setTiposVacuna(response?.data));
    } catch (e) {
      alert.error("Ocurrio un error en la conexion");
    }
  }, []);

  const handleChange = (e) => {
    setTipoVacunaSelected(e.target.value);
  };
  return (
    <>
      <FormControl sx={{ marginTop: 1, marginBottom: 1 }} fullWidth>
        <InputLabel id="Tipo-de-vacuna">Tipo de vacuna:</InputLabel>
        <Select
          labelId="Tipo-de-vacuna"
          id="tipo-de-vauna-select"
          value={tipoVacunaSelected}
          label="Tipo de vacuna"
          onChange={handleChange}
        >
          <MenuItem value={0}>Selecciona un tipo de vacuna</MenuItem>
          {tiposVacuna.map((element, index) => (
            <MenuItem key={index} value={element.id}>
              {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </>
  );
};

export default RegistrarVacuna;
