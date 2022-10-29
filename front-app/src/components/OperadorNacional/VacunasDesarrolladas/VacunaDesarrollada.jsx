import { TableRow, TableCell, SvgIcon, Button } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useState, useContext } from "react";
import CustomModal from "@/components/utils/Modal";
import CustomButton from "@/components/utils/CustomButtom";
import axios from "axios";
import { UserContext } from "@/components/Context/UserContext";
import allUrls from "@/services/backend_url";
import priceParcer from "@/components/utils/pricerParser";
import numberParser from "@/components/utils/numberParser";
import { useAlert } from "react-alert";
const VacunaDesarrollada = ({ vacuna, cargarVacunasDesarrolladas, eliminada = false }) => {
  const [open, setOpen] = useState(false);
  const alert = useAlert();
  const { userSesion } = useContext(UserContext);
  const handleDelete = () => {
    axios
      .delete(
        `${allUrls.eliminarVacunaDesarrollada}?emailOperadorNacional=${userSesion.email}&idVacunaDesarrollada=${vacuna.id}`
      )
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          alert.success("Se elimino con exito");
          cargarVacunasDesarrolladas();
        } else {
          alert.error(response?.data?.errores);
        }
      })
      .catch((e) => alert.error(`Ocurrio un error ${e}`))
      .finally(() => setOpen(false));
  };
  return (
    <TableRow sx={eliminada ? { background: "#FF735E" } : {}}>
      <TableCell>{vacuna.descripcionVacuna}</TableCell>
      <TableCell align="right">{vacuna.descripcionMarcaComercial}</TableCell>
      <TableCell align="right">{numberParser(vacuna.diasDemoraEntrega)}</TableCell>
      <TableCell align="right">{priceParcer(vacuna.precioVacuna)}</TableCell>

      <TableCell align="right">
        {!eliminada && (
          <CustomModal
            title={
              <SvgIcon>
                <DeleteIcon />
              </SvgIcon>
            }
            textColor={"red"}
            color={"error"}
            open={open}
            setOpen={setOpen}
          >
            <form>
              <h4>¿Está seguro que desea eliminar la vacuna?</h4>
              <CustomButton variant={"contained"} color={"error"} onClick={() => setOpen(false)}>
                Cancelar
              </CustomButton>
              <CustomButton color={"success"} variant={"contained"} onClick={handleDelete}>
                Confirmar
              </CustomButton>
            </form>
          </CustomModal>
        )}
      </TableCell>
    </TableRow>
  );
};

export default VacunaDesarrollada;
