import { Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import { useState, useEffect } from "react";
import axios from "axios";
import allUrls from "../../../../services/backend_url";
import { useAlert } from "react-alert";
const SelectPandemia = ({ descripcion, setDescripcion, setDescriptionName }) => {
  const [pandemias, setPandemias] = useState([]);
  const alert = useAlert();
  const handleChange = (evt) => {
    const id = evt.target.value;
    setDescripcion(id);
    const { descripcion } = pandemias.find((element) => element.id === id);
    setDescriptionName(descripcion);
  };
  useEffect(() => {
    try {
      axios.get(`${allUrls.pandemias}GetAll?idTipoVacuna=3`).then((response) => setPandemias(response.data));
    } catch (e) {
      alert.error("Ocurrio un error con el servidor");
    }
  }, []);
  return (
    <>
      <FormControl sx={{ marginTop: 1, marginBottom: 1 }} fullWidth>
        <InputLabel id="Tipo-de-vacuna">Pandemia:</InputLabel>
        <Select
          labelId="Tipo-de-pandemia"
          id="tipo-de-pandemia-select"
          value={descripcion}
          label="Pandemia:"
          onChange={handleChange}
        >
          <MenuItem value={0}>Selecciona una pandemia</MenuItem>
          {pandemias.map((element, index) => (
            <MenuItem key={index} value={element.id}>
              {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </>
  );
};

export default SelectPandemia;
