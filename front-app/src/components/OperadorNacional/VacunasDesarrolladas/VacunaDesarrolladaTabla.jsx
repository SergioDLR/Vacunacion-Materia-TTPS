import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import VacunaDesarrollada from "./VacunaDesarrollada";

const VacunaDesarrolladaTabla = ({ vacunasDesarrolladas = [] }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead>
          <TableRow>
            <TableCell>Vacuna</TableCell>
            <TableCell align="right">Marca comercial</TableCell>
            <TableCell align="right">Dias demora</TableCell>
            <TableCell align="right">Precio</TableCell>
            <TableCell align="right">Acciones</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {vacunasDesarrolladas.map((element, index) => (
            <VacunaDesarrollada vacuna={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default VacunaDesarrolladaTabla;
