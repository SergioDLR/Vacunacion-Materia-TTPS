import { TableRow, TableCell } from "@mui/material";
import CustomModal from "../../utils/Modal";
import ListaReglas from "./ListaReglas";
const Vauna = ({ vacuna }) => {
  return (
    <TableRow>
      <TableCell>{vacuna?.descripcionTipoVacuna}</TableCell>
      <TableCell align="right">{vacuna?.descripcion}</TableCell>
      <TableCell align="right">{vacuna?.descripcionPandemia}</TableCell>
      <TableCell align="right">{vacuna?.cantidadDosis}</TableCell>
      <TableCell align="right">
        <CustomModal title={"Ver"} cerrar={"Cerrar"}>
          <ListaReglas dosis={vacuna?.dosis}></ListaReglas>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default Vauna;
