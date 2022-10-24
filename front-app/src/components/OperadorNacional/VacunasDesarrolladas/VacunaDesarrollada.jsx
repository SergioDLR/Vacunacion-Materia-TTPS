import { TableRow, TableCell, SvgIcon, Button } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useState } from "react";
import CustomModal from "@/components/utils/Modal";

const VacunaDesarrollada = ({ vacuna }) => {
  const [open, setOpen] = useState(false);
  return (
    <TableRow>
      <TableCell>{vacuna.idVacuna}</TableCell>
      <TableCell align="right">{vacuna.idMarcaComercial}</TableCell>
      <TableCell align="right">{vacuna.diasDemoraEntrega}</TableCell>
      <TableCell align="right">{vacuna.precioVacuna} $</TableCell>
      <TableCell align="right">
        <CustomModal
          title={
            <SvgIcon>
              <DeleteIcon />
            </SvgIcon>
          }
          color={"error"}
          open={open}
          setOpen={setOpen}
        >
          <form>
            <h4>¿Esta seguro que desea eliminar la vacuna?</h4>
            <Button variant={"outlined"} onClick={() => setOpen(false)}>
              Cancelar
            </Button>
            <Button color={"error"} variant={"outlined"} onClick={() => console.log("eliminando....")}>
              Confirmar
            </Button>
          </form>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default VacunaDesarrollada;
