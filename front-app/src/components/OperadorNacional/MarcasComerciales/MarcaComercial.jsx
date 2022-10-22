import { TableRow, TableCell, TextField, Button } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useState, useContext } from "react";
import allUrls from "@/services/backend_url";
import axios from "axios";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
const MarcaComercial = ({ marcaComercial, cargarMarcasComerciales }) => {
  const alert = useAlert();
  const { userSesion } = useContext(UserContext);
  const [nuevaDescripcion, setNuevaDescripcion] = useState("");
  const handleSubmit = (event) => {
    event.preventDefault();
    if (nuevaDescripcion.length < 1) return alert.error("Complete el nombre de la marca");
    axios
      .put(allUrls.marcasComercialesModificar, {
        EmailOperadorNacional: userSesion.email,
        DescripcionMarcaComercial: marcaComercial.descripcion,
        DescripcionMarcaComercialNueva: nuevaDescripcion,
      })
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          alert.success("Se modifico con exito");
          cargarMarcasComerciales();
        } else {
          alert.error(response.data.errores);
        }
      });
  };

  const handleChange = (event) => {
    setNuevaDescripcion(event.target.value);
  };

  return (
    <TableRow>
      <TableCell>{marcaComercial.descripcion}</TableCell>
      <TableCell align="right">
        <CustomModal mostrarBotonSubmit={true} title={"Editar"}>
          <h2>Descripcion actual: {marcaComercial.descripcion} </h2>
          <form onSubmit={handleSubmit}>
            <TextField
              id="field-marca"
              label="Nueva descripcion:"
              value={nuevaDescripcion}
              variant="outlined"
              fullWidth
              onChange={handleChange}
            />
            <Button variant="contained" type={"submit"}>
              Actualizar
            </Button>
          </form>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default MarcaComercial;
