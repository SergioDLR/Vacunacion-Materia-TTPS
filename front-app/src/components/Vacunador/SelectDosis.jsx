import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect } from "react";
const SelectDosis = ({ vacuna, dosisSeleccionada, setDosisSeleccionada }) => {
  const handleChangeDosis = (evt) => {
    setDosisSeleccionada(evt.target.value);
  };
  return (
    <FormControl sx={{ marginTop: 1 }} fullWidth>
      <InputLabel id="Tipo-de-vacuna">Dosis:</InputLabel>
      <Select
        labelId="Tipo-de-vacuna"
        id="Tipo-de-vacuna"
        value={dosisSeleccionada}
        label="Dosis:"
        onChange={handleChangeDosis}
        required
      >
        <MenuItem disabled value={0}>
          Selecciona una dosis:
        </MenuItem>
        {vacuna.dosis.map((element, index) => (
          <MenuItem key={index} value={element}>
            {element.descripcionTipoVacuna} - {element.descripcion}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default SelectDosis;
