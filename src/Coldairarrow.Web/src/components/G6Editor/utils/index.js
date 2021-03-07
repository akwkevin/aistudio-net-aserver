import merge from 'lodash/merge'
import pick from 'lodash/pick'
import uniqueId from 'lodash/uniqueId'
import upperFirst from 'lodash/upperFirst'

const toQueryString = obj =>
  Object.keys(obj)
    .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(obj[key])}`)
    .join('&')

const addListener = (target, eventName, handler) => {
  if (typeof handler === 'function') target.on(eventName, handler)
}

const getBox = (x, y, width, height) => {
  const x1 = x + width < x ? x + width : x
  const x2 = x + width > x ? x + width : x
  const y1 = y + height < y ? y + height : y
  const y2 = y + height > y ? y + height : y
  return {
    x1,
    x2,
    y1,
    y2
  }
}

function _mix (dist, obj) {
  for (var key in obj) {
    if (
      obj.hasOwnProperty(key) &&
      key !== 'constructor' &&
      obj[key] !== undefined
    ) {
      dist[key] = obj[key]
    }
  }
}

function mix (dist, src1, src2, src3) {
  if (src1) _mix(dist, src1)
  if (src2) _mix(dist, src2)
  if (src3) _mix(dist, src3)
  return dist
}

export {
  merge,
  pick,
  toQueryString,
  uniqueId,
  upperFirst,
  addListener,
  getBox,
  mix
}
