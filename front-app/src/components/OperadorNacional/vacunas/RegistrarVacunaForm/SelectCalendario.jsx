import { Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import { useState, useEffect } from "react";
import axios from "axios";
import allUrls from "../../../../services/backend_url";
import { useAlert } from "react-alert";
const SelectCalendario = ({ descripcion, setDescripcion }) => {
  const [descripcionesCalendario, setDescipcionesCalendario] = useState([]);
  const alert = useAlert();
  const handleChange = (evt) => {
    setDescripcion(evt.target.value);
  };
  useEffect(() => {
    try {
      axios.get(`${allUrls.calendario}`).then((response) => setDescipcionesCalendario(response?.data));
    } catch (e) {
      alert.error("Ocurrio un error con el servidor");
    }
  }, []);
  return (
    <>
      <FormControl sx={{ marginTop: 1, marginBottom: 1 }} fullWidth>
        <InputLabel id="Tipo-de-vacuna">Tipo:</InputLabel>
        <Select
          labelId="Tipo-de-pandemia"
          id="tipo-de-pandemia-select"
          value={descripcion}
          label="Tipo:"
          onChange={handleChange}
        >
          <MenuItem value={0}>Selecciona un calendario</MenuItem>
          {descripcionesCalendario.map((element, index) => (
            <MenuItem key={index} value={element.descripcion}>
              {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </>
  );
};

export default SelectCalendario;
