import { Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import { useState, useEffect } from "react";
import axios from "axios";
import allUrls from "../../../../services/backend_url";
import { useAlert } from "react-alert";

const SelectAnual = ({ descripcion, setDescripcion }) => {
  const alert = useAlert();
  const [anuales, setAnuales] = useState([]);
  const handleChange = (evt) => {
    setDescripcion(evt.target.value);
  };
  useEffect(() => {
    try {
      axios.get(allUrls.descripcionAnual).then((response) => setAnuales(response.data));
    } catch (e) {
      alert.error("Ocurrio un error con el servidor");
    }
  }, []);
  return (
    <>
      <FormControl sx={{ marginTop: 1, marginBottom: 1 }} fullWidth>
        <InputLabel id="Tipo-de-vacuna">Anual:</InputLabel>
        <Select
          labelId="Tipo-de-pandemia"
          id="tipo-de-pandemia-select"
          value={descripcion}
          label="Anual:"
          onChange={handleChange}
        >
          <MenuItem value={0}>Selecciona una descripcion anual</MenuItem>
          {anuales.map((element, index) => (
            <MenuItem key={index} value={element.descripcion}>
              {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </>
  );
};

export default SelectAnual;
