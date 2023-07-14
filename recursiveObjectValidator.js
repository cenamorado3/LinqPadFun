  let req1 = {};
  let req2 = {};
JSON.stringify(req1) === JSON.stringify(req2) ? console.log('String values are equal!') : RecursiveValidation(req1, req2, "root");

function RecursiveValidation(r1, r2, parentKey){
    let o = TakeLargerBreath(r1, r2)
    for (let [k,v] of Object.entries(o["larger"])){
      if (typeof v === 'object' && o["smaller"].includes(k)){
          RecursiveValidation(v, r2[k], k)
      }
      else if(!o["smaller"].includes(k)){
        console.log(`${k} is not present in the ${o["order"]} object. ${k}'s type is ${typeof v} in parent object ${parentKey}`)
      }

      else if(typeof v !== 'object' && o["smaller"].includes(k) &&
          v != r2[k]
      ){
          console.log(`The value of ${k} is not equal.`)
          let valueDiff = {value1: JSON.stringify(v), value2: JSON.stringify(r2[k])};
          console.log(valueDiff);
      }
    }
}

function TakeLargerBreath(r1, r2)
{
  if(!r2){
    console.log(`${r2} is null`);
  }
  let r1k = Object.keys(r1);
  let r2k = Object.keys(r2);
  if (r1k.length >= r2k.length){
      return {"larger":r1, "smaller":Object.keys(r2), "order":"second"};
  }
  if (r1k.length <= r2k.length){
    return {"larger":r2, "smaller":Object.keys(r1), "order":"first"};
  }
}
