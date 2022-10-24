import { TableRow, TableCell, TextField, Button } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import { useState, useContext } from "react";
import allUrls from "@/services/backend_url";
import axios from "axios";
import { UserContext } from "@/components/Context/UserContext";
import { useAlert } from "react-alert";
import CustomButton from "@/components/utils/CustomButtom";
const MarcaComercial = ({ marcaComercial, cargarMarcasComerciales }) => {
  const alert = useAlert();
  const { userSesion } = useContext(UserContext);
  const [open, setOpen] = useState(false);
  const [nuevaDescripcion, setNuevaDescripcion] = useState("");
  const handleSubmit = (event) => {
    event.preventDefault();
    if (nuevaDescripcion.length < 1) return alert.error("Complete el nombre de la marca");
    try {
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
        })
        .catch((error) => {
          alert.error(`Ocurrio un error del lado del servidor ${error}`);
        })
        .finally(() => setOpen(false));
    } catch (error) {
      alert.error(`Ocurrio un error del lado del servidor ${error}`);
    }
  };

  const handleChange = (event) => {
    setNuevaDescripcion(event.target.value);
  };

  return (
    <TableRow>
      <TableCell>{marcaComercial.descripcion}</TableCell>
      <TableCell align="right">
        <CustomModal mostrarBotonSubmit={true} title={"Editar"} open={open} setOpen={setOpen}>
          <h2>Descripcion actual: {marcaComercial.descripcion} </h2>
          <form onSubmit={handleSubmit}>
            <TextField
              id="field-marca"
              label="Nueva descripcion:"
              value={nuevaDescripcion}
              variant="outlined"
              fullWidth
              required
              onChange={handleChange}
            />
            <div style={{ marginTop: 5 }}>
              <CustomButton variant="contained" color={"error"} type={"submit"} onClick={() => setOpen(false)}>
                Cancelar
              </CustomButton>
              <CustomButton variant="contained" color={"success"} type={"submit"}>
                Actualizar
              </CustomButton>
            </div>
          </form>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default MarcaComercial;
