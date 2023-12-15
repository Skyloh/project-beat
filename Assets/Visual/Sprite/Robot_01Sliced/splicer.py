from PIL import Image

def is_transparent(pixel):
    return pixel[3] == 0

def flood_find_incl_bounds(pixel_array, height, width, start_x, start_y):
    stack = [(start_x, start_y)]
    visited = set()

    bounds = [999999, 999999, -1, -1]

    while stack:
        x, y = stack.pop()

        # if outside array, skip
        if x < 0 or x >= width or y < 0 or y >= height:
            continue

        # if we've seen it or it is transparent, skip
        if (x, y) in visited or is_transparent(pixel_array[x, y]):
            continue

        visited.add((x, y))

        # update max-min bounds
        if bounds[0] > x:
            bounds[0] = x
        if bounds[1] > y:
            bounds[1] = y

        if bounds[2] < x:
            bounds[2] = x
        if bounds[3] < y:
            bounds[3] = y

        # add adjacent positions to the stack
        stack.append((x, y - 1))  # up
        stack.append((x, y + 1))  # down
        stack.append((x - 1, y))  # left
        stack.append((x + 1, y))  # right
    
    bounds[2] += 1
    bounds[3] += 1 # fixes off by 1
    return bounds
    
def crop_and_save(img, count, box):
    cropped_image = img.crop(box)
    # path = 'splicing/output/r' + str(count[0]) + '-' + str(count[1]) + '.png'
    path = 'splicing/output/r' + str(count) + '.png'
    cropped_image.save(path)

def fill_bounds_with_color(img, bounds, color):
    for x in range(bounds[0], bounds[2]):
        for y in range(bounds[1], bounds[3]):
            img.putpixel((x, y), color)

def main():
    # Open the PNG image
    image_path = 'splicing/robot_01.png'
    original_image = Image.open(image_path)

    pixels = original_image.load()
    WIDTH, HEIGHT = original_image.size
    IGNORE_COL = pixels[0, 0]

    # whitewhipe. prolly better way to do this, but eh.
    for x in range(WIDTH):
        for y in range(HEIGHT):  
            if pixels[x, y] == IGNORE_COL:
                pixels[x, y] = (0, 0, 0, 0)

    # rip 1304 (last laser frame) manually due to the visual effect
    # 1390 (shield) has a bad frame that needs to be manually ripped
    # 2057 has toast frames that need to be manually ripped
    ROW_HEIGHTS = [27, 87, 168, 237, 329, 391, 464, 554, 620, 687, 735, 806, 864, 911, 978, 1036, 1084, 1136, 1189, 1251, 1390, 1456, 1545, 1660, 1749, 1813, 1872, 1962, 2057, 2137]
    # manually calculated the rows that have sprites so that they can be ripped better

    count = 0
    for y in ROW_HEIGHTS:
        for x in range(WIDTH):
            if not is_transparent(pixels[x, y]):
                bounding_box = flood_find_incl_bounds(pixels, HEIGHT, WIDTH, x, y)
                crop_and_save(original_image, count, bounding_box)
                fill_bounds_with_color(original_image, bounding_box, (0, 0, 0, 0))
                count += 1
                


main()